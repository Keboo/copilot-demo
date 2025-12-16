export interface ReportHandler {
  (metric: any): void;
}

declare module './reportWebVitals' {
  const reportWebVitals: (onPerfEntry?: ReportHandler) => void;
  export default reportWebVitals;
}
