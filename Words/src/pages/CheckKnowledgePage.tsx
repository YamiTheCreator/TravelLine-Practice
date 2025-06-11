import { useState, useEffect } from "react";
import {
  Button,
  Card,
  CardContent,
  FormControl,
  RadioGroup,
  FormControlLabel,
  Radio,
  Typography,
  LinearProgress,
} from "@mui/material";
import { useNavigate, useLocation } from "react-router-dom";
import useStore from "../store/useStore";
import type { WordType } from "../types/types";
import ResultPage from "./ResultPage";

const CheckKnowledgePage = () => {
  const words = useStore((state) => state.words);
  const navigate = useNavigate();
  const [currentIndex, setCurrentIndex] = useState(0);
  const [selectedWords, setSelectedWords] = useState<WordType[]>([]);
  const [options, setOptions] = useState<string[]>([]);
  const [selectedOption, setSelectedOption] = useState("");
  const [answers, setAnswers] = useState<
    { word: WordType; correct: boolean }[]
  >([]);
  const [completed, setCompleted] = useState(false);
  const location = useLocation();

  useEffect(() => {
    if (location.state?.forceReset) {
      setCurrentIndex(0);
      setAnswers([]);
      setCompleted(false);
    }
    if (words.length < 5) {
      navigate("/", {
        state: { message: "Добавьте хотя бы 5 слов в словарь" },
      });
      return;
    }

    const count = Math.min(5, words.length);
    const shuffled = [...words].sort(() => 0.5 - Math.random());
    setSelectedWords(shuffled.slice(0, count));
  }, [words, navigate, location]);

  useEffect(() => {
    if (selectedWords.length > 0 && currentIndex < selectedWords.length) {
      const currentWord = selectedWords[currentIndex];
      const otherWords = words
        .filter((word) => word.id !== currentWord.id)
        .sort(() => 0.5 - Math.random())
        .slice(0, 4)
        .map((word) => word.english);

      const allOptions = [...otherWords, currentWord.english].sort(
        () => 0.5 - Math.random()
      );
      setOptions(allOptions);
      setSelectedOption("");
    }
  }, [currentIndex, selectedWords, words]);

  const handleNext = () => {
    if (!selectedOption) return;

    const currentWord = selectedWords[currentIndex];
    const isCorrect = selectedOption === currentWord.english;

    setAnswers([
      ...answers,
      {
        word: currentWord,
        correct: isCorrect,
      },
    ]);

    if (currentIndex < selectedWords.length - 1) {
      setCurrentIndex(currentIndex + 1);
    } else {
      setCompleted(true);
    }
  };

  const handleOptionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedOption(event.target.value);
  };

  if (completed) {
    return <ResultPage answers={answers} />;
  }

  if (selectedWords.length === 0 || currentIndex >= selectedWords.length) {
    return <Typography>Загрузка...</Typography>;
  }

  const progress = (currentIndex / selectedWords.length) * 100;

  return (
    <Card>
      <CardContent>
        <LinearProgress variant="determinate" value={progress} sx={{ mb: 2 }} />
        <Typography variant="h6" gutterBottom>
          Что означает слово "{selectedWords[currentIndex].russian}"?
        </Typography>
        <FormControl component="fieldset">
          <RadioGroup value={selectedOption} onChange={handleOptionChange}>
            {options.map((option, index) => (
              <FormControlLabel
                key={index}
                value={option}
                control={<Radio />}
                label={option}
              />
            ))}
          </RadioGroup>
        </FormControl>
      </CardContent>
      <Button
        variant="contained"
        onClick={handleNext}
        disabled={!selectedOption}
        sx={{ m: 2 }}
      >
        {currentIndex < selectedWords.length - 1 ? "Далее" : "Завершить"}
      </Button>
    </Card>
  );
};

export default CheckKnowledgePage;
