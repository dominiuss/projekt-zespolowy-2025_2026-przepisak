import { Link, useLocation, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

export default function Navbar() {
  const location = useLocation();
  const navigate = useNavigate();

  const [username, setUsername] = useState(null);

  // Aktualizacja przy zmianie strony
  useEffect(() => {
    const token = localStorage.getItem("token");
    const storedUser = localStorage.getItem("username");

    setUsername(token && storedUser ? storedUser : null);
  }, [location.pathname]);

  // Aktualizacja globalna gdy localStorage zmieni się (np. po logowaniu)
  useEffect(() => {
    const syncAuth = () => {
      const token = localStorage.getItem("token");
      const storedUser = localStorage.getItem("username");

      setUsername(token && storedUser ? storedUser : null);
    };

    window.addEventListener("storage", syncAuth);
    return () => window.removeEventListener("storage", syncAuth);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("username");

    setUsername(null);
    navigate("/login");
  };

  const isActive = (path) =>
    location.pathname === path
      ? "text-blue-100 font-semibold"
      : "text-white hover:text-blue-200";

  return (
    <nav className="fixed top-0 left-0 w-full bg-blue-600 shadow-md z-50">
      <div className="max-w-6xl mx-auto flex items-center justify-between px-6 py-4">
        
        <Link to="/" className="text-3xl font-semibold text-white">
          Przepisak
        </Link>

        <div className="flex gap-6 text-lg items-center">

          <Link to="/" className={isActive("/")}>
            Strona główna
          </Link>

          {!username && (
            <>
              <Link to="/login" className={isActive("/login")}>
                Logowanie
              </Link>

              <Link to="/register" className={isActive("/register")}>
                Rejestracja
              </Link>
            </>
          )}

          {username && (
            <>
              <span className="text-white font-medium">
                👤 {username}
              </span>

              <button
                onClick={handleLogout}
                className="text-white hover:text-blue-200"
              >
                Wyloguj
              </button>
            </>
          )}

        </div>
      </div>
    </nav>
  );
}
