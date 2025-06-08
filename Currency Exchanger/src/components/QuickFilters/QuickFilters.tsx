import React from 'react';
import { useCurrencyContext } from '../../context/useCurrencyContext';

const QuickFilters: React.FC = () => {
  const { quickFilters, addQuickFilter, removeQuickFilter, applyQuickFilter, currency } = useCurrencyContext();

  const isFilterExists = quickFilters.some(
    (filter) =>
      filter.paymentCurrency === currency.paymentCurrency && filter.purchaseCurrency === currency.purchaseCurrency
  );

  return (
    <div className="mb-4">
      <div className="d-flex align-items-center justify-content-end mb-2">
        <button
          className="btn btn-sm btn-outline-success"
          onClick={addQuickFilter}
          disabled={!currency.paymentCurrency || !currency.purchaseCurrency || isFilterExists}
        >
          Save Current Pair
        </button>
      </div>

      {quickFilters.length > 0 && (
        <div className="d-flex flex-wrap gap-2">
          {quickFilters.map((filter) => (
            <div key={filter.id} className="d-flex align-items-center bg-light rounded-pill px-3 py-1">
              <button className="btn btn-sm p-0 me-2" onClick={() => applyQuickFilter(filter)}>
                {filter.name}
              </button>
              <button className="btn btn-sm p-0 text-danger" onClick={() => removeQuickFilter(filter.id)}>
                x
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default QuickFilters;
