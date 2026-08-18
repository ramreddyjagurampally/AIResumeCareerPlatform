import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

type ResumeItem = {
  id: string;
  fileName: string;
  uploadedAtUtc: string;
};

type JobMatchResult = {
  matchScore: number;
  matchedSkills: string[];
  missingSkills: string[];
  recommendations: string[];
};

export default function JobMatch() {
  const [resumes, setResumes] = useState<ResumeItem[]>([]);
  const [selectedResumeId, setSelectedResumeId] = useState("");
  const [jobDescription, setJobDescription] = useState("");
  const [result, setResult] = useState<JobMatchResult | null>(null);
  const [message, setMessage] = useState("");
  const [matching, setMatching] = useState(false);

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

  const handleMatch = async () => {
    if (!selectedResumeId) {
      setMessage("Please select a resume.");
      return;
    }

    if (!jobDescription.trim()) {
      setMessage("Please enter a job description.");
      return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    setMatching(true);
    setMessage("");
    setResult(null);

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/resumes/${selectedResumeId}/match-job`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            jobDescription,
          }),
        }
      );

      if (response.status === 401) {
        localStorage.removeItem("token");
        navigate("/login");
        return;
      }

      if (!response.ok) {
        const errorText = await response.text();
        setMessage(errorText || "Unable to match job.");
        return;
      }

      const data: JobMatchResult = await response.json();
      setResult(data);
    } catch (error) {
      console.error(error);
      setMessage("Unable to connect to backend.");
    } finally {
      setMatching(false);
    }
  };

  return (
    <div className="job-match-page">
      <div className="job-match-container">
        <div className="job-match-header">
          <p className="eyebrow">Career Matching</p>
          <h1>Match Resume to Job</h1>
          <p>
            Compare your resume against a job description to identify
            matched skills, missing skills, and improvement opportunities.
          </p>
        </div>

        <div className="job-match-card">
          <div className="job-match-field">
            <label>Select Resume</label>

            <select
              value={selectedResumeId}
              onChange={(event) =>
                setSelectedResumeId(event.target.value)
              }
            >
              {resumes.map((resume) => (
                <option key={resume.id} value={resume.id}>
                  {resume.fileName}
                </option>
              ))}
            </select>
          </div>

          <div className="job-match-field">
            <label>Job Description</label>

            <textarea
              rows={14}
              value={jobDescription}
              onChange={(event) =>
                setJobDescription(event.target.value)
              }
              placeholder="Paste the complete job description here..."
            />
          </div>

          <button
            className="primary-action"
            onClick={handleMatch}
            disabled={matching}
          >
            {matching ? "Matching..." : "Match Job"}
          </button>

          {message && (
            <p className="job-match-message">
              {message}
            </p>
          )}
        </div>

        {result && (
          <section className="job-match-result">
            <div className="score-banner">
              <span>Job Match Score</span>
              <strong>{result.matchScore}%</strong>
            </div>

            <div className="result-grid">
              <div>
                <h3>Matched Skills</h3>
                <ul>
                  {result.matchedSkills.map((skill) => (
                    <li key={skill}>{skill}</li>
                  ))}
                </ul>
              </div>

              <div>
                <h3>Missing Skills</h3>
                <ul>
                  {result.missingSkills.map((skill) => (
                    <li key={skill}>{skill}</li>
                  ))}
                </ul>
              </div>

              <div className="recommendations-block">
                <h3>Recommendations</h3>
                <ul>
                  {result.recommendations.map(
                    (recommendation, index) => (
                      <li key={index}>
                        {recommendation}
                      </li>
                    )
                  )}
                </ul>
              </div>
            </div>
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