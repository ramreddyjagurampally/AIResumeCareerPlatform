import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

type UserProfile = {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
};

export default function Profile() {
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [message, setMessage] = useState("Loading profile...");
  const navigate = useNavigate();

  useEffect(() => {
    const loadProfile = async () => {
      const token = localStorage.getItem("token");

      if (!token) {
        navigate("/login");
        return;
      }

      try {
        const response = await fetch(
          `${import.meta.env.VITE_API_URL}/api/users/profile`,
          {
            method: "GET",
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
          setMessage("Unable to load profile.");
          return;
        }

        const data = await response.json();

        setProfile(data);
        setMessage("");
      } catch (error) {
        console.error(error);
        setMessage("Unable to connect to backend.");
      }
    };

    loadProfile();
  }, [navigate]);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  if (!profile) {
    return <p>{message}</p>;
  }

  return (
    <div>
      <h1>My Profile</h1>

      <p>
        <strong>Name:</strong>{" "}
        {profile.firstName} {profile.lastName}
      </p>

      <p>
        <strong>Email:</strong> {profile.email}
      </p>

      <p>
        <strong>User ID:</strong> {profile.userId}
      </p>

      <button onClick={handleLogout}>
        Logout
      </button>
    </div>
  );
}