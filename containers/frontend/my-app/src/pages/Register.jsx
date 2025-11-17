import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const navigate = useNavigate();

  // -----------------------------------------
  // CUSTOM MESSAGES — MOŻESZ ZMIENIAĆ DOWOLNIE
  // -----------------------------------------
  const MSG_SUCCESS = "Konto zostało utworzone! Za chwilę nastąpi przekierowanie.";
  const MSG_GENERIC_ERROR = "Wystąpił błąd podczas rejestracji.";
  const MSG_USERNAME_TAKEN = "Taki użytkownik już istnieje. Wybierz inną nazwę.";
  const MSG_WEAK_PASSWORD = "Hasło jest zbyt słabe. Użyj co najmniej 6 znaków.";
  // -----------------------------------------

  const handleRegister = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      const res = await fetch("http://localhost:5035/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      if (!res.ok) {
        // Odczyt treści z backendu 
        const text = await res.text();

        // Dopasowanie do kodów błędu
        if (text.includes("Username already exists")) {
          setError(MSG_USERNAME_TAKEN);
        } else if (text.includes("password")) {
          setError(MSG_WEAK_PASSWORD);
        } else {
          setError(MSG_GENERIC_ERROR);
        }

        return;
      }

      // Sukces
      setSuccess(MSG_SUCCESS);

      setTimeout(() => {
        navigate("/login");
      }, 1500);

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
            onChange={(e) => setUsername(e.target.value)}
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-400"
          />

          <input
            type="password"
            placeholder="Hasło"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-purple-400"
          />

          <button
            type="submit"
            className="bg-purple-600 text-white p-4 rounded-xl font-semibold hover:bg-purple-700 transition"
          >
            Zarejestruj się
          </button>

          {error && <p className="text-red-600 text-center font-medium">{error}</p>}
          {success && <p className="text-green-600 text-center font-medium">{success}</p>}
        </form>
      </div>
    </div>
  );
}
