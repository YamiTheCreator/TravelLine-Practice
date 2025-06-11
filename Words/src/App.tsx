import { BrowserRouter, Routes, Route } from "react-router-dom";
import MainPage from "./pages/MainPage";
import DictionaryPage from "./pages/DictionaryPage";
import AddWordPage from "./pages/AddWordPage";
import EditWordPage from "./pages/EditWordPage";
import CheckKnowledgePage from "./pages/CheckKnowledgePage";
import Layout from "./components/Layout";
import { Box } from "@mui/material";

function App() {
  return (
    <BrowserRouter>
      <Layout />
      <Box
        sx={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          pt: 8,
          minHeight: "100vh",
          width: "100%",
        }}
      >
        <Routes>
          <Route path="/" element={<MainPage />} />
          <Route path="/dictionary" element={<DictionaryPage />} />
          <Route path="/add" element={<AddWordPage />} />
          <Route path="/edit/:id" element={<EditWordPage />} />
          <Route path="/check" element={<CheckKnowledgePage />} />
        </Routes>
      </Box>
    </BrowserRouter>
  );
}

export default App;
