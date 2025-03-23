import React, { useState, useContext } from 'react';
import { AppContext } from "../context/AppContext";
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Button } from 'primereact/button';

export default function Schedule() {
    const { scheduleResults } = useContext(AppContext);
    console.log(scheduleResults);
    const [viewRoom, setViewRoom] = useState(true);
    const [selectedRoom, setSelectedRoom] = useState(null);
    const [selectedInstructor, setSelectedInstructor] = useState(null);
    const daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
    const teacherNames = [...new Set(scheduleResults.map(item => item.instructor.name))];
    const roomNames = [...new Set(scheduleResults.map(item => item.room.name))];

    // Filter courses based on the selected room or instructor
    const filteredData = scheduleResults.filter((item) => {
        if (viewRoom && selectedRoom) {
            return item.room.name === selectedRoom;
        }
        if (!viewRoom && selectedInstructor) {
            return item.instructor.name === selectedInstructor;
        }
        return true;
    });

    const scheduleByDay = {};
    daysOfWeek.forEach((day) => {
        scheduleByDay[day] = [];
    });

    filteredData.forEach((course) => {
        course.expression.slots.forEach((slot) => {
            const dayIndex = slot.day - 1;
            const dayName = daysOfWeek[dayIndex];

            slot.timeSlots.forEach((timeSlot) => {
                if (!viewRoom) {
                    scheduleByDay[dayName].push({
                        start: timeSlot.start,
                        end: timeSlot.end,
                        course: course.courseSection.displayName,
                        room: course.room.name,
                    });
                } else {
                    scheduleByDay[dayName].push({
                        start: timeSlot.start,
                        end: timeSlot.end,
                        course: course.courseSection.displayName,
                        instructor: course.instructor.name,
                    });
                }
            });
        })
    });

    Object.keys(scheduleByDay).forEach((day) => {
        scheduleByDay[day].sort((a, b) => a.start.localeCompare(b.start));
    });

    const maxRows = Math.max(...Object.values(scheduleByDay).map((list) => list.length));
    const tableData = Array.from({ length: maxRows }, (_, rowIndex) => {
        const row = {};
        daysOfWeek.forEach((day) => {
            row[day] = scheduleByDay[day][rowIndex] || {}; // Empty slot if no class
        });
        return row;
    });

    const cellTemplate = (rowData, column) => {
        const entry = rowData[column.field];
        return entry.course ? (
            <div key={`${entry.course}_${entry.start}-${entry.end}_${viewRoom ? entry.instructor : entry.room}`}>
                <strong>{entry.course}</strong>
                <br />
                {entry.start} - {entry.end}
                <br />
                <small>{viewRoom ? entry.instructor : entry.room}</small>
            </div>
        ) : null;
    };

    const handleToggleView = () => {
        setViewRoom(!viewRoom);
        
        setSelectedRoom(null);
        setSelectedInstructor(null);
    };

    const handleRoomSelect = (roomName) => {
        setSelectedRoom(roomName);
        setSelectedInstructor(null);
    };

    const handleInstructorSelect = (instructorName) => {
        setSelectedInstructor(instructorName);
        setSelectedRoom(null);
    };

    return (
        <div>
            <Button label={`View by ${viewRoom ? 'Instructor' : 'Room'}`} onClick={handleToggleView} />
            {viewRoom ? (
                <div>
                    {roomNames.map(t => <Button label={t} onClick={() => handleRoomSelect(t)} />)}
                </div>
            ) : (
                <div>
                    {teacherNames.map(t => <Button label={t} onClick={() => handleInstructorSelect(t)} />)}
                </div>
            )}
            <DataTable value={tableData} stripedRows>
                {daysOfWeek.map((day) => (
                    <Column key={day} field={day} header={day} body={cellTemplate} />
                ))}
            </DataTable>
        </div>
    );
};