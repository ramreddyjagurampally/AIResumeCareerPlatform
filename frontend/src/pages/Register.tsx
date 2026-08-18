import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const navigate = useNavigate();

  const handleRegister = async (event: React.FormEvent) => {
    event.preventDefault();

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/api/users/register`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            firstName,
            lastName,
            email,
            password,
          }),
        }
      );

      if (!response.ok) {
        setMessage("Registration failed.");
        return;
      }

      setMessage("Registration successful.");

      navigate("/login");
    } catch (error) {
      console.error(error);
      setMessage("Unable to connect to backend.");
    }
  };

  return (
    <div className="login-page">
      <div className="login-card">
        <h1>Create Account</h1>

        <form onSubmit={handleRegister}>
          <div className="form-group">
            <label>First Name</label>
            <input
              type="text"
              value={firstName}
              required
              onChange={(event) =>
                setFirstName(event.target.value)
              }
            />
          </div>

          <div className="form-group">
            <label>Last Name</label>
            <input
              type="text"
              value={lastName}
              required
              onChange={(event) =>
                setLastName(event.target.value)
              }
            />
          </div>

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
            Register
          </button>
        </form>

        <p className="message">
          {message}
        </p>

        <button
          type="button"
          onClick={() => navigate("/login")}
        >
          Already have an account? Login
        </button>
      </div>
    </div>
  );
}