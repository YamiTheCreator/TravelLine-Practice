import { useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Form from "../components/Form";
import useStore from "../store/useStore";
import { Box } from "@mui/material";

const EditWordPage = () => {
  const { id } = useParams();
  const getWordById = useStore((state) => state.getWordById);
  const updateWord = useStore((state) => state.updateWord);
  const navigate = useNavigate();
  const word = getWordById(id || "");

  useEffect(() => {
    if (!word) {
      navigate("/dictionary", { state: { title: "Словарь" } });
    }
  }, [word, navigate]);

  const handleSave = (russian: string, english: string) => {
    if (id) {
      updateWord(id, russian, english);
      navigate("/dictionary", { state: { title: "Словарь" } });
    }
  };

  if (!word) return null;

  return (
    <Box>
      <Form
        onSave={handleSave}
        russianValue={word.russian}
        englishValue={word.english}
      />
    </Box>
  );
};

export default EditWordPage;
