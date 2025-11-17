import { Link, useLocation } from "react-router-dom";

export default function Navbar() {
  const location = useLocation();
  const isActive = (path) =>
    location.pathname === path
      ? "text-blue-100 font-semibold"
      : "text-white hover:text-blue-200";

  return (
    <nav className="fixed top-0 left-0 w-full bg-blue-600 shadow-md z-50">
      <div className="max-w-6xl mx-auto flex items-center justify-between px-6 py-4">
        <Link to="/" className="text-3xl font-semibold text-white">Przepisak</Link>
        <div className="flex gap-6 text-lg">
          <Link to="/" className={isActive("/")}>Strona główna</Link>
          <Link to="/login" className={isActive("/login")}>Logowanie</Link>
          <Link to="/register" className={isActive("/register")}>Rejestracja</Link>
        </div>
      </div>
    </nav>
  );
}
