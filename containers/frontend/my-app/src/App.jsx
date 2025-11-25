import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import RecipeDetails from "./pages/RecipeDetails";
import AddRecipe from "./pages/AddRecipe";
import EditRecipe from "./pages/EditRecipe";

function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/recipe/:id" element={<RecipeDetails />} />
        <Route path="/recipes/add" element={<AddRecipe />} />
        <Route path="/recipes/edit/:id" element={<EditRecipe />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
