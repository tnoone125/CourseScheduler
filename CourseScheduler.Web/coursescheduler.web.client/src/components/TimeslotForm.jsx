import { useContext } from "react";
import { AppContext } from "../context/AppContext";
export default function TimeslotForm() {
    const { rooms, teachers, currentStep, setCurrentStep } = useContext(AppContext);

    return (
        <>
            <p>More to come soon. Summary of your inputs: </p>
            <p>Instructors: </p>
            <pre style={{ whiteSpace: 'pre-wrap', wordWrap: 'break-word' }}>{JSON.stringify(teachers)}</pre>

            <p>Rooms: </p>
            <pre style={{ whiteSpace: 'pre-wrap', wordWrap: 'break-word' }}>{JSON.stringify(rooms)}</pre>

            <div className="button-section">
                <button onClick={() => setCurrentStep(currentStep - 1)}>Go Back</button>
            </div>
        </>
    );
}