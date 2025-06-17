import { useCurrencyContext } from '../../context/useCurrencyContext';

const LoadingDisplay: React.FC = () => {
  const { loading } = useCurrencyContext();
  if (loading.initial) {
    return (
      <div className="container mt-5">
        <div className="text-center">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-2 text-muted">Loading application...</p>
        </div>
      </div>
    );
  }
};

export default LoadingDisplay;
