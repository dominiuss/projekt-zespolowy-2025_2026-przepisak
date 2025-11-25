import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import RecipeForm from "../components/RecipeForm";

export default function EditRecipe() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchRecipe = async () => {
      const res = await fetch(`http://10.6.57.161:5035/api/recipes/${id}`);
      const data = await res.json();
      setRecipe(data);
      setLoading(false);
    };
    fetchRecipe();
  }, [id]);

  const handleEdit = async (form) => {
    const token = localStorage.getItem("token");

    if (!token) {
      alert("Musisz być zalogowany!");
      navigate("/login");
      return;
    }

    // KLUCZOWE — struktura musi zawierać recipeIngredients
    const payload = {
      id: Number(id),
      title: form.title,
      description: form.description,
      instructions: form.instructions,
      preparationTime: form.preparationTime,
      cookTime: form.cookTime,
      servings: form.servings,
      categoryName: form.categoryName,
      cuisine: form.cuisine,
      imageUrl: form.imageUrl,
      recipeIngredients: form.recipeIngredients   // <-- najważniejsze
    };

    const res = await fetch(`http://10.6.57.161:5035/api/recipes/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(payload)
    });

    if (!res.ok) {
      alert("Błąd podczas edycji przepisu!");
      return;
    }

    navigate(`/recipes/${id}`);
  };

  if (loading) return <p className="text-center mt-20">Ładowanie...</p>;

  return (
    <div className="pt-28 max-w-3xl mx-auto p-6">
      <h1 className="text-3xl font-bold mb-4 text-orange-700">Edytuj przepis</h1>
      <RecipeForm
        initialData={recipe}
        buttonText="Zapisz zmiany"
        onSubmit={handleEdit}
      />
    </div>
  );
}
