import { Button, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";

const MainPage = () => {
  const navigate = useNavigate();
  return (
    <Stack spacing={2} direction="row">
      <Button
        variant="contained"
        onClick={() => navigate("/dictionary", { state: { title: "Словарь" } })}
      >
        Заполнить словарь
      </Button>
      <Button
        variant="outlined"
        onClick={() =>
          navigate("/check", { state: { title: "Проверка знаний" } })
        }
      >
        Проверить знания
      </Button>
    </Stack>
  );
};

export default MainPage;
