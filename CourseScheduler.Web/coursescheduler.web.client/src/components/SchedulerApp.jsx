import { AppContext } from "../context/AppContext";
import InstructorForm from "./InstructorForm";
import RoomForm from "./RoomsForm";
import TimeslotForm from "./TimeslotForm";
import { useContext } from 'react';
import { AnimatePresence } from 'framer-motion';
import AnimationWrapper from "./AnimationWrapper";
import '../css/App.css';
import "primereact/resources/themes/viva-light/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import fplogo from "../assets/fp_logo.jpg";

export default function App() {
    const { currentStep } = useContext(AppContext);
    return (
        <>
            <img src={fplogo} />
            <h2>Fordham Preparatory School - Course Scheduler</h2>
            <AnimatePresence mode="wait">
                {currentStep === 1 && (
                    <AnimationWrapper keyName="instructor">
                        <InstructorForm />
                    </AnimationWrapper>
                )}
                {currentStep === 2 && (
                    <AnimationWrapper keyName="room">
                        <RoomForm />
                    </AnimationWrapper>
                )}
                {currentStep === 3 && (
                    <AnimationWrapper keyName="timeslots">
                        <TimeslotForm />
                    </AnimationWrapper>
                )}
            </AnimatePresence>
        </>
    );
}