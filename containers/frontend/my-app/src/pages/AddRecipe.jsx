import { useNavigate } from "react-router-dom";
import RecipeForm from "../components/RecipeForm";

export default function AddRecipe() {
  const navigate = useNavigate();

  const handleAdd = async (form) => {
    const token = localStorage.getItem("token");

    if (!token) {
      alert("Musisz być zalogowany!");
      navigate("/login");
      return;
    }

    // form zawiera już recipeIngredients — NIE usuwaj tego
    const payload = {
      title: form.title,
      description: form.description,
      instructions: form.instructions,
      preparationTime: form.preparationTime,
      cookTime: form.cookTime,
      servings: form.servings,
      categoryName: form.categoryName,
      cuisine: form.cuisine,
      imageUrl: form.imageUrl,
      recipeIngredients: form.recipeIngredients   
    };

    const res = await fetch("http://10.6.57.161:5035/api/recipes", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(payload)
    });

    if (!res.ok) {
      alert("Błąd podczas dodawania przepisu!");
      return;
    }

    const data = await res.json();
    navigate(`/recipes/${data.id}`);
  };

  return (
    <div className="pt-28 max-w-3xl mx-auto p-6">
      <h1 className="text-3xl font-bold mb-4 text-orange-700">Dodaj przepis</h1>
      <RecipeForm buttonText="Dodaj przepis" onSubmit={handleAdd} />
    </div>
  );
}
