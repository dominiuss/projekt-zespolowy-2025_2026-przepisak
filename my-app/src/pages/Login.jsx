import { Link } from "react-router-dom";

export default function Login() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-blue-200 pt-16">
      <div className="bg-white shadow-2xl rounded-3xl p-10 w-full max-w-md mx-4">
        <h2 className="text-3xl font-bold text-center text-blue-700 mb-8">
          Logowanie
        </h2>

        <form className="flex flex-col gap-5">
          <input
            type="email"
            placeholder="Adres e-mail"
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-400"
          />
          <input
            type="password"
            placeholder="Hasło"
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-blue-400"
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
