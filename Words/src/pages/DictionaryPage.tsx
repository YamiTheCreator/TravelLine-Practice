import { useState } from "react";
import {
  IconButton,
  Menu,
  MenuItem,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  Button,
  Box,
} from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import { useNavigate } from "react-router-dom";
import useStore from "../store/useStore";

const DictionaryPage = () => {
  const words = useStore((state) => state.words);
  const deleteWord = useStore((state) => state.deleteWord);
  const navigate = useNavigate();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedWordId, setSelectedWordId] = useState<string | null>(null);

  const handleMenuOpen = (
    event: React.MouseEvent<HTMLElement>,
    wordId: string
  ) => {
    setAnchorEl(event.currentTarget);
    setSelectedWordId(wordId);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedWordId(null);
  };

  const handleEdit = () => {
    if (selectedWordId) {
      navigate(`/edit/${selectedWordId}`, {
        state: { from: "/dictionary", title: "Редактирование слова" },
      });
    }
    handleMenuClose();
  };

  const handleDelete = () => {
    if (selectedWordId) {
      deleteWord(selectedWordId);
    }
    handleMenuClose();
  };

  return (
    <Box
      sx={{
        display: "flex",
        flexFlow: "column",
      }}
    >
      <Button
        variant="contained"
        onClick={() =>
          navigate("/add", { state: { title: "Добавление слова" } })
        }
        sx={{ mb: 2 }}
      >
        Добавить слово
      </Button>
      {words.length === 0 ? (
        <Typography>Словарь пуст. Добавьте первое слово.</Typography>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Русский</TableCell>
                <TableCell>Английский</TableCell>
                <TableCell align="right">Действия</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {words.map((word) => (
                <TableRow key={word.id}>
                  <TableCell>{word.russian}</TableCell>
                  <TableCell>{word.english}</TableCell>
                  <TableCell align="right">
                    <IconButton
                      onClick={(e) => handleMenuOpen(e, word.id)}
                      aria-label="actions"
                    >
                      <MoreVertIcon />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={handleEdit}>Редактировать</MenuItem>
        <MenuItem onClick={handleDelete}>Удалить</MenuItem>
      </Menu>
    </Box>
  );
};

export default DictionaryPage;
