import { useActionState, useState, useEffect } from "react";
import { RatingPanel } from "./RatingPanel";

interface FormData {
  cleanliness: number;
  service: number;
  speed: number;
  location: number;
  communication: number;
  name: string;
  comment: string;
}

interface Review extends FormData {
  id: string;
  timestamp: number;
}

const initialFormData: FormData = {
  cleanliness: -1,
  service: -1,
  speed: -1,
  location: -1,
  communication: -1,
  name: "",
  comment: "",
};

const submitForm = async (
  prevState: { success: boolean } | undefined,
  formData: FormData
): Promise<{ success: boolean }> => {
  if (!formData.name.trim()) {
    return { success: false };
  }

  const convertedFormData = {
    ...formData,
    cleanliness: formData.cleanliness >= 0 ? formData.cleanliness + 1 : -1,
    service: formData.service >= 0 ? formData.service + 1 : -1,
    speed: formData.speed >= 0 ? formData.speed + 1 : -1,
    location: formData.location >= 0 ? formData.location + 1 : -1,
    communication:
      formData.communication >= 0 ? formData.communication + 1 : -1,
  };

  const review: Review = {
    ...convertedFormData,
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

  const handleRatingChange = (field: keyof FormData) => (value: number) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleInputChange =
    (field: keyof FormData) =>
    (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
      const value = e.target.value;
      setFormData((prev) => ({ ...prev, [field]: value }));
    };

  const isFormValid = (): boolean => {
    const hasName = formData.name.trim().length > 0;
    const allRatingsSelected = [
      formData.cleanliness,
      formData.service,
      formData.speed,
      formData.location,
      formData.communication,
    ].every((rating) => rating >= 0);

    return hasName && allRatingsSelected;
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
    <div className="container border rounded-3 p-5">
      <h2 className="mb-5">Помогите нам сделать процесс бронирования лучше</h2>
      <form onSubmit={handleSubmit} className="d-flex flex-column gap-4">
        <div className="d-flex flex-column gap-3 px-3">
          <RatingPanel
            name="cleanliness"
            value={formData.cleanliness}
            onChange={handleRatingChange("cleanliness")}
          >
            Чистота
          </RatingPanel>
          <RatingPanel
            name="service"
            value={formData.service}
            onChange={handleRatingChange("service")}
          >
            Сервис
          </RatingPanel>
          <RatingPanel
            name="speed"
            value={formData.speed}
            onChange={handleRatingChange("speed")}
          >
            Скорость
          </RatingPanel>
          <RatingPanel
            name="location"
            value={formData.location}
            onChange={handleRatingChange("location")}
          >
            Место
          </RatingPanel>
          <RatingPanel
            name="communication"
            value={formData.communication}
            onChange={handleRatingChange("communication")}
          >
            Культура речи
          </RatingPanel>
        </div>

        <div className="mb-3 position-relative px-3">
          <label
            htmlFor="name"
            className="position-absolute top-0 start-0 translate-middle text-secondary ms-5 bg-white"
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

        <div className="mb-3 px-3">
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
          className={`btn align-self-end text-align-center w-50 ${
            isFormValid() ? "btn-primary" : "btn-secondary"
          }`}
        >
          Отправить
        </button>
      </form>
    </div>
  );
};
