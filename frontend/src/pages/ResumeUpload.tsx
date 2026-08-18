import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function ResumeUpload() {
  const [file, setFile] = useState<File | null>(null);
  const [message, setMessage] = useState("");
  const [uploading, setUploading] = useState(false);

  const navigate = useNavigate();

  const handleUpload = async (event: React.FormEvent) => {
    event.preventDefault();

    if (!file) {
      setMessage("Please select a resume file.");
      return;
    }

    const token = localStorage.getItem("token");

    if (!token) {
      navigate("/login");
      return;
    }

    const formData = new FormData();
    formData.append("file", file);

    setUploading(true);
    setMessage("");

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/resumes/upload`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        }
      );

      if (response.status === 401) {
        localStorage.removeItem("token");
        navigate("/login");
        return;
      }

      if (!response.ok) {
        setMessage(`Upload failed: ${response.status}`);
        return;
      }

      setMessage("Resume uploaded successfully.");
      setFile(null);
    } catch (error) {
      console.error(error);
      setMessage("Unable to connect to backend.");
    } finally {
      setUploading(false);
    }
  };

  return (
    <div className="upload-page">
      <div className="upload-card">
        <div className="upload-header">
          <p className="eyebrow">Resume Management</p>
          <h1>Upload Resume</h1>
          <p>
            Upload your PDF resume so you can analyze it and compare it
            against job descriptions.
          </p>
        </div>

        <form onSubmit={handleUpload}>
          <label className="upload-box">
            <input
              type="file"
              accept=".pdf"
              onChange={(event) => {
                const selectedFile =
                  event.target.files?.[0] ?? null;

                setFile(selectedFile);
                setMessage("");
              }}
            />

            <span className="upload-box-title">
              Choose a PDF resume
            </span>

            <span className="upload-box-subtitle">
              Click here to select a file from your computer
            </span>
          </label>

          {file && (
            <div className="selected-file">
              <span>Selected file</span>
              <strong>{file.name}</strong>
            </div>
          )}

          <button
            className="primary-action"
            type="submit"
            disabled={uploading}
          >
            {uploading ? "Uploading..." : "Upload Resume"}
          </button>
        </form>

        {message && (
          <p className="upload-message">
            {message}
          </p>
        )}

        <div className="page-actions">
          <button
            onClick={() => navigate("/resumes")}
          >
            My Resumes
          </button>

          <button
            onClick={() => navigate("/dashboard")}
          >
            Back to Dashboard
          </button>
        </div>
      </div>
    </div>
  );
}