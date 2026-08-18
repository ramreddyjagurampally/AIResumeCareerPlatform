import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

type ResumeItem = {
  id: string;
  fileName: string;
  uploadedAtUtc: string;
};

type ResumeAnalysisResult = {
  atsScore: number;
  skills: string[];
  strengths: string[];
  missingSections: string[];
  suggestions: string[];
};

export default function ResumeAnalysis() {
  const [resumes, setResumes] = useState<ResumeItem[]>([]);
  const [selectedResumeId, setSelectedResumeId] = useState("");
  const [analysis, setAnalysis] =
    useState<ResumeAnalysisResult | null>(null);

  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(false);

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

        if (data.length > 0) {
          setSelectedResumeId(data[0].id);
        }
      } catch (error) {
        console.error(error);
        setMessage("Unable to connect to backend.");
      }
    };

    loadResumes();
  }, [navigate]);

  const handleAnalyze = async () => {
    if (!selectedResumeId) {
      setMessage("Please select a resume.");
      return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    setLoading(true);
    setMessage("");
    setAnalysis(null);

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/resumes/${selectedResumeId}/analyze`,
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
        const errorText = await response.text();

        setMessage(
          errorText || "Resume analysis failed."
        );

        return;
      }

      const data: ResumeAnalysisResult =
        await response.json();

      setAnalysis(data);
    } catch (error) {
      console.error(error);

      setMessage(
        "Unable to connect to backend."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="analysis-page">
      <div className="analysis-card">
        <h1>Resume Analysis</h1>

        <label>Select Resume</label>

        <select
          value={selectedResumeId}
          onChange={(event) =>
            setSelectedResumeId(event.target.value)
          }
        >
          {resumes.map((resume) => (
            <option
              key={resume.id}
              value={resume.id}
            >
              {resume.fileName}
            </option>
          ))}
        </select>

        <button
          onClick={handleAnalyze}
          disabled={loading}
        >
          {loading
            ? "Analyzing..."
            : "Analyze Resume"}
        </button>

        {message && <p>{message}</p>}

        {analysis && (
          <div className="analysis-results">
            <h2>
              ATS Score: {analysis.atsScore}/100
            </h2>

            <h3>Skills Detected</h3>
            <ul>
              {analysis.skills.map((skill) => (
                <li key={skill}>
                  {skill}
                </li>
              ))}
            </ul>

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
        )}

        <button
          onClick={() =>
            navigate("/dashboard")
          }
        >
          Back to Dashboard
        </button>
      </div>
    </div>
  );
}