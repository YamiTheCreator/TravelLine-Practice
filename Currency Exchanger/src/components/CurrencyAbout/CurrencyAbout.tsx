// components/CurrencyAbout.tsx
import React from 'react';
import { useCurrencyContext } from '../../context/useCurrencyContext';
import CurrencyDetails from '../CurrencyDetails/CurrencyDetails';

const CurrencyAbout: React.FC = () => {
  const { showDetails, toggleDetails, loading } = useCurrencyContext();

  return (
    <>
      <button className="btn btn-outline-primary btn-sm mt-2" onClick={toggleDetails} disabled={loading.details}>
        <i className={`bi ${showDetails ? 'bi-eye-slash' : 'bi-eye'} me-1`}></i>
        {showDetails ? 'Hide' : 'Show'} Currency Details
      </button>
      <CurrencyDetails />
    </>
  );
};

export default CurrencyAbout;
