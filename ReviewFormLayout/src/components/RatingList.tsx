import { useActionState, useState, useEffect } from "react";
import { RatingPanel } from "./RatingPanel";

interface FormData {
  rating: number;
  name: string;
  comment: string;
}

interface Review extends FormData {
  id: string;
  timestamp: number;
}

const initialFormData: FormData = {
  rating: -1,
  name: "",
  comment: "",
};

const submitForm = async (
  prevState: { success: boolean } | undefined,
  formData: FormData
): Promise<{ success: boolean }> => {
  if (!formData.name.trim() || formData.rating === -1) {
    return { success: false };
  }

  const review: Review = {
    ...formData,
    rating: formData.rating + 1, // Конвертируем из 0-4 в 1-5
    id: Date.now().toString(),
    timestamp: Date.now(),
  };

  const existingReviews = JSON.parse(localStorage.getItem("reviews") || "[]");
  const updatedReviews = [...existingReviews, review];
  localStorage.setItem("reviews", JSON.stringify(updatedReviews));

  return { success: true };
};

export const RatingList = () => {
  const [state, submitAction] = useActionState(submitForm, undefined);
  const [formData, setFormData] = useState<FormData>(initialFormData);
  const [isSubmitted, setIsSubmitted] = useState(false);

  const handleRatingChange = (value: number) => {
    setFormData((prev) => ({ ...prev, rating: value }));
  };

  const handleInputChange =
    (field: keyof FormData) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      const value = e.target.value;
      setFormData((prev) => ({ ...prev, [field]: value }));
    };

  const isFormValid = (): boolean => {
    return formData.name.trim().length > 0 && formData.rating >= 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitted(true);

    if (!isFormValid()) {
      return;
    }

    await submitAction(formData);
  };

  useEffect(() => {
    if (state?.success && isSubmitted) {
      setFormData(initialFormData);
      setIsSubmitted(false);
    }
  }, [state?.success, isSubmitted]);

  return (
    <div className="container">
      <div className="border rounded-3 p-5 mb-5">
        <h2 className="mb-5 text-center">
          Помогите нам сделать процесс бронирования лучше
        </h2>
        <form onSubmit={handleSubmit} className="d-flex flex-column gap-4">
          <RatingPanel value={formData.rating} onChange={handleRatingChange} />

          <div className="mb-3 position-relative">
            <label
              htmlFor="name"
              className="position-absolute top-0 start-0 translate-middle text-secondary ms-3 bg-white px-2"
            >
              Имя*
            </label>
            <input
              type="text"
              className="form-control"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleInputChange("name")}
              placeholder="Как вас зовут?"
            />
          </div>

          <div className="mb-3">
            <textarea
              className="form-control"
              id="comment"
              name="comment"
              rows={4}
              value={formData.comment}
              onChange={handleInputChange("comment")}
              placeholder="Напишите, что понравилось, что было непонятно"
              style={{ resize: "none" }}
            />
          </div>

          <button
            type="submit"
            disabled={!isFormValid()}
            className={`btn align-self-center w-50 ${
              isFormValid() ? "btn-primary" : "btn-secondary"
            }`}
          >
            Отправить
          </button>
        </form>
      </div>
    </div>
  );
};
