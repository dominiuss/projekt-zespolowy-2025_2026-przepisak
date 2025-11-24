import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const navigate = useNavigate();

  // ----------------------------
  // Custom messages
  // ----------------------------
  const MSG_SUCCESS =
    "Konto zostało utworzone! Za chwilę nastąpi przekierowanie.";
  const MSG_GENERIC_ERROR = "Wystąpił błąd podczas rejestracji.";
  const MSG_USERNAME_TAKEN = "Taki użytkownik już istnieje. Wybierz inną nazwę.";
  const MSG_WEAK_PASSWORD =
    "Hasło musi mieć min. 8 znaków, 1 wielką literę i 1 cyfrę.";
  const MSG_SHORT_USERNAME =
    "Nazwa użytkownika musi zawierać co najmniej 4 znaki.";
  // ----------------------------

  // VALIDATION
  const validateUsername = (u) => u.length >= 4;

  const validatePassword = (pwd) => {
    const minLength = pwd.length >= 8;
    const hasUpper = /[A-Z]/.test(pwd);
    const hasDigit = /\d/.test(pwd);

    return minLength && hasUpper && hasDigit;
  };

  // ----------------------------
  // Live validation — USERNAME
  // ----------------------------
  const handleUsernameChange = (e) => {
    const newValue = e.target.value;

    setUsername(newValue);
    setSuccess("");

    if (newValue.length === 0) {
      setError(""); 
      return;
    }

    if (!validateUsername(newValue)) {
      setError(MSG_SHORT_USERNAME);
    } else {
      setError("");
    }
  };

  // ----------------------------
  // Live validation — PASSWORD
  // ----------------------------
  const handlePasswordChange = (e) => {
    const newValue = e.target.value;

    setPassword(newValue);
    setSuccess("");

    if (newValue.length === 0) {
      setError("");
      return;
    }

    if (!validatePassword(newValue)) {
      setError(MSG_WEAK_PASSWORD);
    } else {
      setError("");
    }
  };

  // ----------------------------
  // FORM SUBMIT
  // ----------------------------
  const handleRegister = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    // 1. Validate USERNAME first
    if (!validateUsername(username)) {
      setError(MSG_SHORT_USERNAME);
      return;
    }

    // 2. Validate PASSWORD second
    if (!validatePassword(password)) {
      setError(MSG_WEAK_PASSWORD);
      return;
    }

    // 3. Send to backend
    try {
      const res = await fetch("http://10.6.57.161:5035/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      if (!res.ok) {
        const text = await res.text();

        if (text.includes("Username already exists")) {
          setError(MSG_USERNAME_TAKEN);
        } else {
          setError(MSG_GENERIC_ERROR);
        }

        return;
      }

      // Success
      setSuccess(MSG_SUCCESS);

      setTimeout(() => navigate("/login"), 1500);

    } catch (err) {
      setError(MSG_GENERIC_ERROR);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-purple-50 to-purple-200 pt-16">
      <div className="bg-white shadow-2xl rounded-3xl p-10 w-full max-w-md mx-4">
        <h2 className="text-3xl font-bold text-center text-purple-700 mb-8">
          Rejestracja
        </h2>

        <form onSubmit={handleRegister} className="flex flex-col gap-5">
          <input
            type="text"
            placeholder="Nazwa użytkownika"
            value={username}
            onChange={handleUsernameChange}
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-400"
          />

          <input
            type="password"
            placeholder="Hasło"
            value={password}
            onChange={handlePasswordChange}
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-400"
          />

          <button
            type="submit"
            className="bg-purple-600 text-white p-4 rounded-xl font-semibold hover:bg-purple-700 transition"
          >
            Zarejestruj się
          </button>
          </form>

      {error && (
        <p className="text-red-600 text-center font-medium">{error}</p>
      )}
      {success && (
        <p className="text-green-600 text-center font-medium">{success}</p>
      )}

      <p className="text-center text-gray-600 mt-6">
        Masz już konto?{" "}
        <a
          href="/login"
          className="text-purple-600 hover:underline font-medium"
        >
          Zaloguj się
        </a>
      </p>

    </div>
  </div>
);
}
