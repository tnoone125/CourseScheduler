import { createContext, useState } from "react";

export const AppContext = createContext();

export function AppProvider({ children }) {
    const [teachers, setTeachers] = useState([
        { name: "", department: "", courseMin: "", courseMax: "" }
    ]);
    const [rooms, setRooms] = useState([
        { name: "", studentCapacity: "", permittedDepartments: [] }
    ]);
    const [timeslots, setTimeslots] = useState([]);

    const [courses, setCourses] = useState([
        { name: "", displayName: "", department: "", numberOfSections: "", enrollment: "", preferredTimeslots: [] }
    ]);

    const [statusMessage, setStatusMessage] = useState("");

    const [scheduleResults, setScheduleResults] = useState([]);

    return (
        <AppContext.Provider value={{
            teachers,
            setTeachers,
            rooms,
            setRooms,
            timeslots,
            setTimeslots,
            courses,
            setCourses,
            statusMessage,
            setStatusMessage,
            scheduleResults,
            setScheduleResults,
        }}>
            {children}
        </AppContext.Provider>
    );
}