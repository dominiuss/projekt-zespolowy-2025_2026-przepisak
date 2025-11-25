import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";

export default function Home() {
  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");
  const [searchResults, setSearchResults] = useState([]);

  const navigate = useNavigate();
  const token = localStorage.getItem("token");
  const currentUser = localStorage.getItem("username");

  useEffect(() => {
    const fetchAll = async () => {
      setLoading(true);
      try {
        const res = await fetch("http://10.6.57.161:5035/api/recipes");
        if (!res.ok) throw new Error("Błąd pobierania danych");
        const data = await res.json();
        setRecipes(data);
      } catch (err) {
        console.error(err);
        setRecipes([]);
      } finally {
        setLoading(false);
      }
    };
    fetchAll();
  }, []);

  const handleSearch = async () => {
    if (!search.trim()) {
      setSearchResults([]);
      return;
    }

    setLoading(true);
    try {
      const [byNameRes, byTitleRes] = await Promise.all([
        fetch(`http://10.6.57.161:5035/api/recipes/search/name?name=${search}`),
        fetch(`http://10.6.57.161:5035/api/recipes/search/title?title=${search}`)
      ]);

      if (!byNameRes.ok || !byTitleRes.ok) throw new Error("Błąd wyszukiwania");

      const [byName, byTitle] = await Promise.all([byNameRes.json(), byTitleRes.json()]);
      const combined = [...byName, ...byTitle];
      const uniqueRecipes = Array.from(new Map(combined.map(r => [r.id, r])).values());
      setSearchResults(uniqueRecipes);
    } catch (err) {
      console.error(err);
      setSearchResults([]);
    } finally {
      setLoading(false);
    }
  };

  const displayedRecipes = searchResults.length > 0 ? searchResults : recipes;

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-blue-200 pt-24 px-4 flex flex-col items-center">

      {/* Dodaj nowy przepis — tylko dla zalogowanego */}
      {token && (
        <div className="w-full max-w-6xl mb-4 flex justify-end">
          <Link
            to="/add-recipe"
            className="bg-green-600 text-white px-4 py-2 rounded-xl hover:bg-green-700 transition"
          >
            Dodaj nowy przepis
          </Link>
        </div>
      )}

      <h1 className="text-4xl font-bold text-blue-700 mb-6 text-center">Przepisy</h1>

      {/* Panel wyszukiwania */}
      <div className="w-full max-w-md mb-8 flex gap-2">
        <input
          type="text"
          placeholder="Szukaj po nazwie lub autorze..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="flex-1 p-4 rounded-2xl border shadow-md focus:outline-none focus:ring-2 focus:ring-blue-400 text-lg"
          onKeyDown={(e) => { if(e.key === 'Enter') handleSearch(); }}
        />
        <button
          onClick={handleSearch}
          className="bg-blue-600 text-white px-4 rounded-xl hover:bg-blue-700 transition"
        >
          Szukaj
        </button>
      </div>

      {/* Lista przepisów */}
      {loading ? (
        <p className="text-center text-gray-600">Ładowanie...</p>
      ) : displayedRecipes.length === 0 ? (
        <p className="text-center text-gray-500">Brak pasujących przepisów.</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 max-w-6xl w-full">
          {displayedRecipes.map((recipe) => (
            <div key={recipe.id} className="bg-white shadow-lg rounded-2xl overflow-hidden hover:scale-105 transform transition relative">
              <Link to={`/recipes/${recipe.id}`}>
                <img
                  src={recipe.imageUrl || "https://via.placeholder.com/400x300"}
                  alt={recipe.title}
                  className="w-full h-48 object-cover rounded-2xl"
                />
              </Link>
              <div className="p-4">
                <h2 className="text-xl font-semibold mb-1">{recipe.title}</h2>
                <p className="text-sm text-gray-500 mb-1">Autor: {recipe.authorName}</p>

                {/* Wyświetlanie oceny */}
                {recipe.averageRating ? (
                  <p className="text-yellow-600 font-medium">
                    ⭐ {recipe.averageRating.toFixed(1)} ({recipe.ratingsCount})
                  </p>
                ) : (
                  <p className="text-gray-400">Brak ocen</p>
                )}
              </div>

              {/* Edytuj przepis — tylko dla autora */}
              {currentUser === recipe.authorName && (
                <Link
                  to={`/edit-recipe/${recipe.id}`}
                  className="absolute top-2 right-2 bg-yellow-500 text-white px-3 py-1 rounded hover:bg-yellow-600 transition"
                >
                  Edytuj
                </Link>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
