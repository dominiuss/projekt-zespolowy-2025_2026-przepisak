import { useState, useEffect } from "react";

export default function RecipeForm({ initialData = {}, onSubmit, buttonText }) {
  const [form, setForm] = useState({
    title: initialData.title || "",
    description: initialData.description || "",
    instructions: initialData.instructions || "",
    preparationTime: initialData.preparationTime || "",
    cookTime: initialData.cookTime || "",
    servings: initialData.servings || "",
    categoryName: initialData.categoryName || "",
    cuisine: initialData.cuisine || "",
    imageUrl: initialData.imageUrl || "",
  });

  //  LISTA SKŁADNIKÓW Z BACKENDU
  const [ingredients, setIngredients] = useState([]);

  //  SKŁADNIK AKTUALNIE WYBIERANY
  const [currentIngredientId, setCurrentIngredientId] = useState("");
  const [currentAmount, setCurrentAmount] = useState("");

  //  SKŁADNIKI WYBRANE DO PRZEPISU
  const [selectedIngredients, setSelectedIngredients] = useState(
    initialData.recipeIngredients || []
  );

  //  POBIERANIE SKŁADNIKÓW Z BACKENDU
  useEffect(() => {
    const loadIngredients = async () => {
      try {
        const res = await fetch("http://10.6.57.161:5035/api/recipes/ingredient");
        const data = await res.json();
        setIngredients(data);
      } catch {
        console.error("Nie udało się pobrać składników.");
      }
    };
    loadIngredients();
  }, []);

  //  dodawanie składnika
  const addIngredient = () => {
    if (!currentIngredientId || !currentAmount) return;

    const ing = ingredients.find((i) => i.id === Number(currentIngredientId));
    if (!ing) return;

    setSelectedIngredients((prev) => [
      ...prev,
      {
        ingredientId: ing.id,
        name: ing.name,
        quantity: currentAmount,
      },
    ]);

    setCurrentIngredientId("");
    setCurrentAmount("");
  };

  //  usuwanie składnika
  const removeIngredient = (id) => {
    setSelectedIngredients((prev) =>
      prev.filter((i) => i.ingredientId !== id)
    );
  };

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  //  WYSYŁANIE — dodajemy recipeIngredients
  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ ...form, recipeIngredients: selectedIngredients });
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-4">

      {/* --- POLA TEKSTOWE --- */}
      {Object.keys(form).map((field) => (
        <input
          key={field}
          name={field}
          value={form[field]}
          onChange={handleChange}
          placeholder={field}
          className="p-3 border rounded-lg"
          required={field !== "imageUrl"}
        />
      ))}

      {/* --- WYBÓR SKŁADNIKÓW --- */}
      <h3 className="text-lg font-semibold mt-4">Składniki</h3>

      <div className="flex gap-3">
        {/* select składnika */}
        <select
          value={currentIngredientId}
          onChange={(e) => setCurrentIngredientId(e.target.value)}
          className="border p-2 rounded w-1/2"
        >
          <option value="">-- wybierz składnik --</option>
          {ingredients.map((ing) => (
            <option key={ing.id} value={ing.id}>
              {ing.name}
            </option>
          ))}
        </select>

        {/* ilość */}
        <input
          type="text"
          placeholder="np. 200g"
          className="border p-2 rounded w-1/2"
          value={currentAmount}
          onChange={(e) => setCurrentAmount(e.target.value)}
        />

        <button
          type="button"
          onClick={addIngredient}
          className="bg-green-600 text-white px-4 rounded"
        >
          Dodaj
        </button>
      </div>

      {/* --- WYŚWIETLANIE LISTY DODANYCH SKŁADNIKÓW --- */}
      <ul className="mt-2">
        {selectedIngredients.map((ing) => (
          <li
            key={ing.ingredientId}
            className="flex justify-between bg-gray-100 p-2 rounded mt-1"
          >
            <span>{ing.name} – {ing.amount}</span>
            <button
              className="text-red-600"
              type="button"
              onClick={() => removeIngredient(ing.ingredientId)}
            >
              Usuń
            </button>
          </li>
        ))}
      </ul>

      {/* --- SUBMIT --- */}
      <button
        type="submit"
        className="bg-orange-600 text-white p-3 rounded-xl font-semibold hover:bg-orange-700 transition mt-4"
      >
        {buttonText}
      </button>
    </form>
  );
}
