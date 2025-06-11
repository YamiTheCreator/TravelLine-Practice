import { Box, Button, Stack, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

type FormProps = {
  russianValue: string;
  englishValue: string;
  onSave: (russian: string, english: string) => void;
  onCancel?: () => void;
};

const Form = ({ russianValue = "", englishValue = "", onSave }: FormProps) => {
  const [russian, setRussian] = useState(russianValue);
  const [english, setEnglish] = useState(englishValue);

  const isValid = Boolean(russian.trim() && english.trim());

  const navigate = useNavigate();

  useEffect(() => {
    setRussian(russianValue);
    setEnglish(englishValue);
  }, [russianValue, englishValue]);

  const handleSave = (e: React.FormEvent) => {
    e.preventDefault();
    if (!isValid) return;
    onSave(russian.trim(), english.trim());
  };

  return (
    <Box>
      <form onSubmit={handleSave}>
        <Stack spacing={3}>
          {[
            { type: "russian", label: "Слово на русском языке" },
            { type: "english", label: "Перевод на английский язык" },
          ].map((field) => (
            <TextField
              key={field.type}
              label={field.label}
              variant="outlined"
              fullWidth
              value={field.type === "russian" ? russian : english}
              onChange={(e) =>
                field.type === "russian"
                  ? setRussian(e.target.value)
                  : setEnglish(e.target.value)
              }
              required
            />
          ))}

          <Stack spacing={2} direction="row" justifyContent="flex-end">
            <Button
              variant="contained"
              color="primary"
              type="submit"
              disabled={!isValid}
            >
              Сохранить
            </Button>
            <Button variant="outlined" onClick={() => navigate(-1)}>
              Отменить
            </Button>
          </Stack>
        </Stack>
      </form>
    </Box>
  );
};

export default Form;
