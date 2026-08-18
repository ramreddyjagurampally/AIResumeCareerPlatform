import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const navigate = useNavigate();

  const handleLogin = async (event: React.FormEvent) => {
    event.preventDefault();

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/users/login`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            email,
            password,
          }),
        }
      );

      if (!response.ok) {
        setMessage("Login failed.");
        return;
      }

      const data = await response.json();

      localStorage.setItem("token", data.token);

      setMessage("Login successful.");

      navigate("/dashboard");
    } catch (error) {
      console.error(error);

      setMessage(
        "Unable to connect to the backend."
      );
    }
  };

  return (
    <div className="login-page">
      <div className="login-card">
        <h1>Login</h1>

        <form onSubmit={handleLogin}>
          <div className="form-group">
            <label>Email</label>

            <input
              type="email"
              value={email}
              required
              onChange={(event) =>
                setEmail(event.target.value)
              }
            />
          </div>

          <div className="form-group">
            <label>Password</label>

            <input
              type="password"
              value={password}
              required
              onChange={(event) =>
                setPassword(event.target.value)
              }
            />
          </div>

          <button
            className="login-button"
            type="submit"
          >
            Login
          </button>
        </form>

        <p className="message">
          {message}
        </p>

        <button
          type="button"
          onClick={() =>
            navigate("/register")
          }
        >
          Create Account
        </button>
      </div>
    </div>
  );
}