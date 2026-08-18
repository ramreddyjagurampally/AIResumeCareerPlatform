import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";

import Login from "./pages/Login";
import Register from "./pages/Register";
import Profile from "./pages/Profile";
import Dashboard from "./pages/Dashboard";
import ResumeUpload from "./pages/ResumeUpload";
import MyResumes from "./pages/MyResumes";
import JobMatch from "./pages/JobMatch";
import ResumeAnalysis from "./pages/ResumeAnalysis";

import "./App.css";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/register"
          element={<Register />}
        />

        <Route
          path="/login"
          element={<Login />}
        />

        <Route
          path="/dashboard"
          element={<Dashboard />}
        />

        <Route
          path="/profile"
          element={<Profile />}
        />

        <Route
          path="/resume-upload"
          element={<ResumeUpload />}
        />

        <Route
          path="/resumes"
          element={<MyResumes />}
        />

        <Route
          path="/resume-analysis"
          element={<ResumeAnalysis />}
        />

        <Route
          path="/job-match"
          element={<JobMatch />}
        />

        <Route
          path="*"
          element={<Navigate to="/login" />}
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;