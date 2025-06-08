// components/ErrorDisplay.tsx
import React from 'react';
import { useCurrencyContext } from '../../context/useCurrencyContext';

const ErrorDisplay: React.FC = () => {
  const { error, currencies } = useCurrencyContext();

  if (error && currencies.length === 0) {
    return (
      <div className="container mt-5">
        <div className="alert alert-danger d-flex align-items-center" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-2"></i>
          <div>
            {error}
            <button className="btn btn-sm btn-outline-danger ms-2" onClick={() => window.location.reload()}>
              Retry
            </button>
          </div>
        </div>
      </div>
    );
  }

  return null;
};

export default ErrorDisplay;
