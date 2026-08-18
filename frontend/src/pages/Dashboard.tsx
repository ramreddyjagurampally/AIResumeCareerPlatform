import { useNavigate } from "react-router-dom";

export default function Dashboard() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  const features = [
    {
      title: "View Profile",
      description: "Review your account details and user information.",
      action: () => navigate("/profile"),
    },
    {
      title: "Upload Resume",
      description: "Upload a new PDF resume for analysis and matching.",
      action: () => navigate("/resume-upload"),
    },
    {
      title: "My Resumes",
      description: "View your uploaded resumes and extract resume text.",
      action: () => navigate("/resumes"),
    },
    {
      title: "Analyze Resume",
      description: "Check ATS score, detected skills, strengths, and suggestions.",
      action: () => navigate("/resume-analysis"),
    },
    {
      title: "Match Jobs",
      description: "Compare your resume with a job description and find skill gaps.",
      action: () => navigate("/job-match"),
    },
  ];

  return (
    <div className="dashboard-shell">
      <header className="dashboard-header">
        <div>
          <h1>AI Resume Career Platform</h1>
          <p>Resume analysis, ATS insights, and job matching in one place.</p>
        </div>

        <button
          className="logout-button"
          onClick={handleLogout}
        >
          Logout
        </button>
      </header>

      <main className="dashboard-content">
        <section className="dashboard-hero">
          <h2>Career Dashboard</h2>
          <p>
            Upload your resume, analyze your skills, and compare your
            experience against job descriptions.
          </p>
        </section>

        <section className="feature-grid">
          {features.map((feature) => (
            <button
              key={feature.title}
              className="feature-card"
              onClick={feature.action}
            >
              <h3>{feature.title}</h3>
              <p>{feature.description}</p>
              <span>Open →</span>
            </button>
          ))}
        </section>
      </main>
    </div>
  );
}