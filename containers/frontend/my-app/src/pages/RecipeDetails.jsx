import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";

export default function RecipeDetails() {
  const { id } = useParams();
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

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

        {/* Instrukcje */}
        <h2 className="text-2xl font-semibold text-orange-700 mb-2">
          Instrukcje
        </h2>
        <p className="text-gray-700 whitespace-pre-line">{recipe.instructions}</p>
      </div>
    </div>
  );
}
