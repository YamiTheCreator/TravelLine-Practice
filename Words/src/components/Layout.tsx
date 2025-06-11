import { useLocation, useNavigate } from "react-router-dom";
import { ChevronLeft } from "@mui/icons-material";
import { Box, Typography, IconButton } from "@mui/material";

const Layout = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const handleGoBack = () => {
    if (!location.state?.from) {
      navigate("/");
      return;
    }
    navigate(-1);
  };

  if (location.pathname === "/") return null;

  return (
    <Box
      component="header"
      sx={{
        display: "flex",
        alignItems: "center",
        gap: 2,
        p: 2,
        position: "fixed", // Меняем sticky на fixed
        top: 0,
        left: 0,
        right: 0,
        backgroundColor: "background.paper",
        zIndex: 1000,
        boxShadow: 1,
      }}
    >
      <IconButton
        onClick={handleGoBack}
        aria-label="Go back"
        sx={{ marginRight: 1 }}
      >
        <ChevronLeft />
      </IconButton>
      <Typography variant="h6" component="h1">
        {location.state?.title || "Back"}
      </Typography>
    </Box>
  );
};

export default Layout;
