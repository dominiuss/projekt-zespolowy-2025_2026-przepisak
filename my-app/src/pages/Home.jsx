import { useEffect, useState } from "react";

export default function Home() {
  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Podmień URL na adres Twojego backendu
    fetch("http://localhost:5000/api/recipes")
      .then((res) => {
        if (!res.ok) throw new Error("Błąd pobierania danych");
        return res.json();
      })
      .then((data) => {
        setRecipes(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
      });
  }, []);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center text-xl text-gray-600">
        Ładowanie przepisów...
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 pt-20 px-4">
      <h1 className="text-4xl font-bold text-center text-blue-700 mb-8">Przepisy</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 max-w-6xl mx-auto">
        {recipes.map((recipe) => (
          <div
            key={recipe.id}
            className="bg-white shadow-lg rounded-2xl overflow-hidden hover:scale-105 transform transition"
          >
            <img
              src={recipe.image || "https://via.placeholder.com/400x300"}
              alt={recipe.title}
              className="w-full h-48 object-cover"
            />
            <div className="p-4">
              <h2 className="text-xl font-semibold mb-2">{recipe.title}</h2>
              <p className="text-gray-600">{recipe.description}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
