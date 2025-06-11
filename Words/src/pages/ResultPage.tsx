import { Button, Card, CardContent, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import type { WordType } from "../types/types";

interface ResultPageProps {
  answers: { word: WordType; correct: boolean }[];
}

const ResultPage = ({ answers }: ResultPageProps) => {
  const navigate = useNavigate();
  const correctCount = answers.filter((a) => a.correct).length;
  const totalCount = answers.length;

  const handleRetry = () => {
    navigate("/check", { replace: true, state: { forceReset: true } });
  };

  return (
    <Card>
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Результаты проверки
        </Typography>
        <Typography variant="body1" gutterBottom>
          Правильных ответов: {correctCount}
        </Typography>
        <Typography variant="body1" gutterBottom>
          Неправильных ответов: {totalCount - correctCount}
        </Typography>
        <Typography variant="body1" gutterBottom>
          Всего слов: {totalCount}
        </Typography>
        <Button variant="contained" onClick={handleRetry} sx={{ mr: 2 }}>
          Пройти ещё раз
        </Button>
        <Button variant="outlined" onClick={() => navigate("/")}>
          На главную
        </Button>
      </CardContent>
    </Card>
  );
};

export default ResultPage;
