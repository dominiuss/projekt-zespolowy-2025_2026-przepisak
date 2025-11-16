import { Link } from "react-router-dom";

export default function Register() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-green-50 to-green-200 pt-16">
      <div className="bg-white shadow-2xl rounded-3xl p-10 w-full max-w-md mx-4">
        <h2 className="text-3xl font-bold text-center text-green-700 mb-8">
          Rejestracja
        </h2>

        <form className="flex flex-col gap-5">
          <input
            type="text"
            placeholder="Imię i nazwisko"
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-green-400"
          />
          <input
            type="email"
            placeholder="Adres e-mail"
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-green-400"
          />
          <input
            type="password"
            placeholder="Hasło"
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-green-400"
          />
          <input
            type="password"
            placeholder="Powtórz hasło"
            className="p-4 border rounded-xl focus:outline-none focus:ring-2 focus:ring-green-400"
          />
          <button
            type="submit"
            className="bg-green-600 text-white p-4 rounded-xl font-semibold hover:bg-green-700 transition"
          >
            Zarejestruj się
          </button>
        </form>

        <p className="text-center text-gray-600 mt-6">
          Masz już konto?{" "}
          <Link to="/login" className="text-green-600 hover:underline font-medium">
            Zaloguj się
          </Link>
        </p>
      </div>
    </div>
  );
}
