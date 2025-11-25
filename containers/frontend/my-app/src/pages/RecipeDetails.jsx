import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";


export default function RecipeDetails() {
  const { id } = useParams();
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  // --- Oceny ---
  const [ratings, setRatings] = useState([]);
  const [score, setScore] = useState(0);
  const [comment, setComment] = useState("");
  const [ratingMessage, setRatingMessage] = useState(null);
  const [ratingError, setRatingError] = useState(null);
  const token = localStorage.getItem("token");

  useEffect(() => {
    const fetchRecipe = async () => {
      try {
        const res = await fetch(`http://10.6.57.161:5035/api/recipes/${id}`);

        if (!res.ok) {
          throw new Error("Nie znaleziono przepisu");
        }

        const data = await res.json();
        setRecipe(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchRecipe();
  }, [id]);
  // Pobieranie ocen przepisu
  const fetchRatings = async () => {
    try {
      const res = await fetch(`http://10.6.57.161:5035/api/ratings/${id}`);
      if (!res.ok) return;
      const data = await res.json();
      setRatings(data);
    } catch (err) {
      console.error("Błąd pobierania ocen:", err);
    }
  };

  useEffect(() => {
    fetchRatings();
  }, [id]);

  const submitRating = async (e) => {
    e.preventDefault();
    setRatingMessage(null);
    setRatingError(null);

    if (!token) {
      setRatingError("Musisz być zalogowany, aby ocenić przepis.");
      return;
    }

    try {
      const res = await fetch("http://10.6.57.161:5035/api/ratings", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          recipeId: id,
          score: Number(score),
          comment,
        }),
      });

      const data = await res.json();

      if (res.ok) {
        setRatingMessage("Ocena została dodana!");
        setScore(0);
        setComment("");

        // odświeżamy listę ocen
        fetchRatings();
      } else {
        setRatingError(data.message || "Wystąpił błąd podczas dodawania oceny.");
      }
    } catch (err) {
      setRatingError("Błąd klienta podczas dodawania oceny.");
    }
  };

  if (loading)
    return (
      <p className="text-center text-gray-600 mt-20 text-xl">Ładowanie...</p>
    );

  if (error)
    return (
      <p className="text-center text-red-600 mt-20 text-xl">{error}</p>
    );

  return (
    <div className="min-h-screen bg-gradient-to-br from-yellow-50 to-orange-200 p-8 pt-28 flex justify-center">
      <div className="bg-white shadow-2xl rounded-3xl p-8 max-w-4xl w-full">
        
        {/* Powrót */}
        <Link
          to="/"
          className="text-orange-600 font-semibold hover:underline"
        >
          ← Powrót do listy przepisów
        </Link>

        {/* Tytuł */}
        <h1 className="text-4xl font-bold text-orange-700 mt-4 mb-4">
          {recipe.title}
        </h1>

        {/* Autor + meta */}
        <p className="text-gray-600 mb-4">
          Autor: <span className="font-medium">{recipe.authorName}</span>
        </p>

        <p className="text-gray-500 text-sm mb-6">
          Dodano: {new Date(recipe.createdAt).toLocaleDateString()}
          {recipe.updatedAt &&
            ` • Aktualizacja: ${new Date(recipe.updatedAt).toLocaleDateString()}`}
        </p>

        {/* Zdjęcie */}
        <img
          src={
            recipe.imageUrl && recipe.imageUrl.trim() !== ""
              ? recipe.imageUrl
              : "https://via.placeholder.com/600x400?text=Brak+zdjęcia"
          }
          alt={recipe.title}
          className="w-full h-80 object-cover rounded-2xl mb-6 shadow-md"
        />

        {/* Informacje o przepisie */}
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-6 mb-8">
          <div className="p-4 bg-orange-50 rounded-xl shadow">
            <h3 className="font-semibold text-orange-700">Kategoria</h3>
            <p>{recipe.categoryName}</p>
          </div>

          <div className="p-4 bg-orange-50 rounded-xl shadow">
            <h3 className="font-semibold text-orange-700">Kuchnia</h3>
            <p>{recipe.cuisine}</p>
          </div>

          <div className="p-4 bg-orange-50 rounded-xl shadow">
            <h3 className="font-semibold text-orange-700">Czas przygotowania</h3>
            <p>{recipe.preparationTime} min</p>
          </div>

          <div className="p-4 bg-orange-50 rounded-xl shadow">
            <h3 className="font-semibold text-orange-700">Czas gotowania</h3>
            <p>{recipe.cookTime} min</p>
          </div>

          <div className="p-4 bg-orange-50 rounded-xl shadow">
            <h3 className="font-semibold text-orange-700">Porcje</h3>
            <p>{recipe.servings}</p>
          </div>
        </div>

        {/* Opis */}
        <h2 className="text-2xl font-semibold text-orange-700 mb-2">
          Opis
        </h2>
        <p className="text-gray-700 mb-6">{recipe.description}</p>
          {/* Składniki */}
          <h2 className="text-2xl font-semibold text-orange-700 mt-8 mb-3">
            Składniki
          </h2>

          {recipe.recipeIngredients && recipe.recipeIngredients.length > 0 ? (
            <ul className="list-disc list-inside text-gray-800 mb-8">
              {recipe.recipeIngredients.map((ing, idx) => (
                <li key={idx}>
                  <span className="font-medium">{ing.name}</span>
                  {ing.quantity && ` – ${ing.quantity}`}
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-gray-600 italic mb-8">
              Brak danych o składnikach.
            </p>
          )}
        {/* Instrukcje */}
        <h2 className="text-2xl font-semibold text-orange-700 mb-2">
          Instrukcje
        </h2>
        <p className="text-gray-700 whitespace-pre-line">{recipe.instructions}</p>
        
        {/* Dodawanie oceny */}
        <div className="mt-12 p-6 bg-gray-100 rounded-lg shadow-md">
          <h2 className="text-2xl font-semibold mb-4">Dodaj swoją ocenę</h2>

          {ratingMessage && (
            <p className="text-green-600 font-semibold mb-3">{ratingMessage}</p>
          )}

          {ratingError && (
            <p className="text-red-600 font-semibold mb-3">{ratingError}</p>
          )}

          <form onSubmit={submitRating} className="flex flex-col gap-4">
            <div>
              <label className="font-semibold">Ocena (1–5):</label>
              <input
                type="number"
                min="1"
                max="5"
                value={score}
                onChange={(e) => setScore(e.target.value)}
                className="border rounded p-2 w-20 ml-2"
                required
              />
            </div>

            <div>
              <label className="font-semibold">Komentarz (opcjonalnie):</label>
              <textarea
                value={comment}
                onChange={(e) => setComment(e.target.value)}
                className="border rounded p-2 w-full min-h-[80px]"
                placeholder="Podziel się swoją opinią..."
              />
            </div>

            <button
              type="submit"
              className="bg-orange-600 text-white px-4 py-2 rounded hover:bg-orange-700 w-fit"
            >
              Dodaj ocenę
            </button>
          </form>
        </div>
        {/* Lista ocen */}
        <div className="mt-10">
          <h2 className="text-2xl font-semibold mb-4">Opinie użytkowników</h2>

          {ratings.length === 0 && (
            <p className="text-gray-600 italic">
              Brak ocen — bądź pierwszą osobą, która oceni ten przepis!
            </p>
          )}

          <div className="flex flex-col gap-4">
            {ratings.map((r) => (
              <div key={r.id} className="bg-white p-4 rounded-lg shadow border">
                <div className="flex items-center gap-2">
                  <span className="font-bold">Ocena:</span>
                  <span className="text-yellow-500 text-lg">
                    {"★".repeat(r.score)}{"☆".repeat(5 - r.score)}
                  </span>
                </div>

                {r.comment && (
                  <p className="mt-2 text-gray-700">{r.comment}</p>
                )}

                <p className="text-sm text-gray-500 mt-2">
                  Wystawiono: {new Date(r.createdAt).toLocaleString()}
                </p>
              </div>
            ))}
          </div>
        </div>


      </div>
    </div>
  );
}
