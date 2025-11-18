import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";

export default function Login() {
  const [formData, setFormData] = useState({ username: "", password: "" });
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const res = await fetch("http://10.6.57.161:5035/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData)
      });

      if (!res.ok) {
        const msg = await res.text();
        alert("Błąd logowania: " + msg);
        return;
      }

      const data = await res.json();
      localStorage.setItem("token", data.token);
      navigate("/"); // przekierowanie na stronę główną po loginie

    } catch (err) {
      alert("Błąd połączenia z backendem");
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-blue-200 pt-16">
      <div className="bg-white shadow-2xl rounded-3xl p-10 w-full max-w-md mx-4">
        <h2 className="text-3xl font-bold text-center text-blue-700 mb-8">
          Logowanie
        </h2>

        <form className="flex flex-col gap-5" onSubmit={handleSubmit}>
          <input
            name="userName"
            placeholder="Nazwa użytkownika"
            onChange={handleChange}
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-400"
            required
          />

          <input
            type="password"
            name="password"
            placeholder="Hasło"
            onChange={handleChange}
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-400"
            required
          />

          <button
            type="submit"
            className="bg-blue-600 text-white p-4 rounded-xl font-semibold hover:bg-blue-700 transition"
          >
            Zaloguj się
          </button>
        </form>

        <p className="text-center text-gray-600 mt-6">
          Nie masz konta?{" "}
          <Link to="/register" className="text-blue-600 hover:underline font-medium">
            Zarejestruj się
          </Link>
        </p>
      </div>
    </div>
  );
}
