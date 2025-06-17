import CurrencyProvider from './context/CurrencyProvider';
import ErrorDisplay from './components/ErrorDisplay/ErrorDisplay';
import CurrencyExchanger from './components/CurrencyExchanger/CurrencyExchanger';
import LoadingDisplay from './components/LoadingDisplay/LoadingDisplay';

function App() {
  return (
    <CurrencyProvider>
      <div className="container mt-4">
        <div className="row justify-content-center p-3">
          <div className="col-md-8 col-lg-6">
            <ErrorDisplay />
            <LoadingDisplay />
            <CurrencyExchanger />
          </div>
        </div>
      </div>
    </CurrencyProvider>
  );
}

export default App;
