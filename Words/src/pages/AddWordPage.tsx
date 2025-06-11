import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Form from "../components/Form";
import useStore from "../store/useStore";
import { Box } from "@mui/material";

const AddWordPage = () => {
  const addWord = useStore((state) => state.addWord);
  const navigate = useNavigate();

  const handleSave = (russian: string, english: string) => {
    addWord(russian, english);
    navigate("/dictionary", { state: { title: "Словарь" } });
  };

  useEffect(() => {
    document.title = "Добавление слова";
  }, []);

  return (
    <Box>
      <Form onSave={handleSave} russianValue="" englishValue="" />
    </Box>
  );
};

export default AddWordPage;
