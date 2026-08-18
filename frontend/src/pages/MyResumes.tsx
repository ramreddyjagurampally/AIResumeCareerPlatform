import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

type ResumeItem = {
  id: string;
  fileName: string;
  uploadedAtUtc: string;
};

type ResumeAnalysis = {
  atsScore: number;
  skills: string[];
  strengths: string[];
  missingSections: string[];
  suggestions: string[];
};

export default function MyResumes() {
  const [resumes, setResumes] = useState<ResumeItem[]>([]);
  const [message, setMessage] = useState("Loading resumes...");
  const [resumeText, setResumeText] = useState("");
  const [analysis, setAnalysis] =
    useState<ResumeAnalysis | null>(null);

  const [extractingId, setExtractingId] =
    useState<string | null>(null);

  const [analyzingId, setAnalyzingId] =
    useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    const loadResumes = async () => {
      const token = localStorage.getItem("token");

      if (!token) {
        navigate("/login");
        return;
      }

      try {
        const response = await fetch(
          `${import.meta.env.VITE_API_URL}/api/resumes`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (response.status === 401) {
          localStorage.removeItem("token");
          navigate("/login");
          return;
        }

        if (!response.ok) {
          setMessage("Unable to load resumes.");
          return;
        }

        const data = await response.json();

        setResumes(data);
        setMessage("");
      } catch (error) {
        console.error(error);
        setMessage("Unable to connect to backend.");
      }
    };

    loadResumes();
  }, [navigate]);

  const handleExtractText = async (resumeId: string) => {
    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    setExtractingId(resumeId);
    setResumeText("");
    setAnalysis(null);

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/resumes/${resumeId}/text`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.status === 401) {
        localStorage.removeItem("token");
        navigate("/login");
        return;
      }

      if (!response.ok) {
        setResumeText("Unable to extract resume text.");
        return;
      }

      const data = await response.json();
      setResumeText(data.text);
    } catch (error) {
      console.error(error);
      setResumeText("Unable to connect to backend.");
    } finally {
      setExtractingId(null);
    }
  };

  const handleAnalyze = async (resumeId: string) => {
    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    setAnalyzingId(resumeId);
    setAnalysis(null);
    setResumeText("");

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/resumes/${resumeId}/analyze`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.status === 401) {
        localStorage.removeItem("token");
        navigate("/login");
        return;
      }

      if (!response.ok) {
        return;
      }

      const data: ResumeAnalysis =
        await response.json();

      setAnalysis(data);
    } catch (error) {
      console.error(error);
    } finally {
      setAnalyzingId(null);
    }
  };

  return (
    <div className="resumes-page">
      <div className="resumes-container">
        <div className="resumes-header">
          <div>
            <p className="eyebrow">Resume Management</p>
            <h1>My Resumes</h1>
            <p>
              Review your uploaded resumes, extract text, and run ATS-style
              analysis.
            </p>
          </div>

          <button
            className="primary-action resume-upload-button"
            onClick={() => navigate("/resume-upload")}
          >
            Upload New Resume
          </button>
        </div>

        {message && <p>{message}</p>}

        {!message && resumes.length === 0 && (
          <div className="empty-state">
            <h3>No resumes uploaded yet</h3>
            <p>
              Upload your first PDF resume to begin analyzing your experience.
            </p>
          </div>
        )}

        <div className="resume-grid">
          {resumes.map((resume) => (
            <div className="resume-card" key={resume.id}>
              <div>
                <p className="resume-label">PDF Resume</p>
                <h3>{resume.fileName}</h3>

                <p className="resume-date">
                  Uploaded{" "}
                  {new Date(
                    resume.uploadedAtUtc
                  ).toLocaleString()}
                </p>
              </div>

              <div className="resume-card-actions">
                <button
                  onClick={() =>
                    handleExtractText(resume.id)
                  }
                  disabled={extractingId === resume.id}
                >
                  {extractingId === resume.id
                    ? "Extracting..."
                    : "Extract Text"}
                </button>

                <button
                  className="dark-button"
                  onClick={() =>
                    handleAnalyze(resume.id)
                  }
                  disabled={analyzingId === resume.id}
                >
                  {analyzingId === resume.id
                    ? "Analyzing..."
                    : "Analyze Resume"}
                </button>
              </div>
            </div>
          ))}
        </div>

        {analysis && (
          <section className="resume-result-card">
            <div className="score-banner">
              <span>ATS Score</span>
              <strong>{analysis.atsScore}/100</strong>
            </div>

            <div className="result-grid">
              <div>
                <h3>Skills Detected</h3>
                <ul>
                  {analysis.skills.map((skill) => (
                    <li key={skill}>{skill}</li>
                  ))}
                </ul>
              </div>

              <div>
                <h3>Strengths</h3>
                <ul>
                  {analysis.strengths.map(
                    (strength, index) => (
                      <li key={index}>
                        {strength}
                      </li>
                    )
                  )}
                </ul>
              </div>

              <div>
                <h3>Missing Sections</h3>
                <ul>
                  {analysis.missingSections.map(
                    (section, index) => (
                      <li key={index}>
                        {section}
                      </li>
                    )
                  )}
                </ul>
              </div>

              <div>
                <h3>Suggestions</h3>
                <ul>
                  {analysis.suggestions.map(
                    (suggestion, index) => (
                      <li key={index}>
                        {suggestion}
                      </li>
                    )
                  )}
                </ul>
              </div>
            </div>
          </section>
        )}

        {resumeText && (
          <section className="resume-result-card">
            <h2>Extracted Resume Text</h2>

            <pre className="resume-text">
              {resumeText}
            </pre>
          </section>
        )}

        <button
          className="back-button"
          onClick={() => navigate("/dashboard")}
        >
          Back to Dashboard
        </button>
      </div>
    </div>
  );
}