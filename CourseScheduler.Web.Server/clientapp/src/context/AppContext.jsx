import { createContext, useState } from "react";

export const AppContext = createContext();

export function AppProvider({ children }) {
    const [teachers, setTeachers] = useState([
        { name: "", department: "", minCourses: "", maxCourses: "" }
    ]);
    const [rooms, setRooms] = useState([
        { name: "", studentCapacity: "", permittedDepartments: [] }
    ]);
    const [timeslots, setTimeslots] = useState([]);

    const [courses, setCourses] = useState([
        { name: "", displayName: "", numberOfSections: "", enrollment: "", preferredTimeslots: [] }
    ]);
    const [currentStep, setCurrentStep] = useState(1);

    return (
        <AppContext.Provider value={{
            teachers,
            setTeachers,
            rooms,
            setRooms,
            timeslots,
            setTimeslots,
            currentStep,
            setCurrentStep,
            courses,
            setCourses,
        }}>
            {children}
        </AppContext.Provider>
    );
}