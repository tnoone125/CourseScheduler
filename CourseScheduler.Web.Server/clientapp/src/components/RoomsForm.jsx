import { useState, useContext } from "react";
import { AppContext } from "../context/AppContext";
import { getDepartmentsFromInstructors } from '../util/departments'
import { MultiSelect } from 'primereact/multiselect';
import { InputText } from 'primereact/inputtext';
import { InputNumber } from 'primereact/inputnumber';
import '../css/App.css';

export default function RoomInputForm() {
    const { rooms, teachers, setRooms, currentStep, setCurrentStep } = useContext(AppContext);
    const departments = getDepartmentsFromInstructors(teachers);

    const addRoom = () => {
        setRooms([...rooms, { name: "", studentCapacity: "", permittedDepartments: [] }]);
    };

    const handleChange = (index, field, value) => {
        const updatedRooms = [...rooms];
        let v;
        if (field === "studentCapacity" && value !== "") {
            v = Number(value);
        } else {
            v = value;
        }
        updatedRooms[index][field] = v;

        setRooms(updatedRooms);
    };

    const enableButton = rooms.every(r => r.name && r.name !== "" && r.studentCapacity && r.studentCapacity > 0)
        && [...new Set(rooms.map(r => r.name))].length === rooms.length;

    const hasDuplicates = new Set(rooms.map(r => r.name)).size !== rooms.length;

    return (
        <>
            <div className="p-3 space-y-3">
                {rooms.map((room, index) => (
                    <div key={index} className="flex space-x-2 entry-row">
                        <InputText
                            placeholder="Room Name"
                            value={room.name}
                            onChange={(e) => handleChange(index, "name", e.target.value)}
                        />
                        <InputNumber
                            min={0}
                            step={1}
                            placeholder="Student Capacity"
                            value={room.studentCapacity}
                            onKeyDown={(e) => {
                                if (e.key === '-' || e.key === 'e' || e.key === '.') {
                                    e.preventDefault();
                                }
                            }}
                            mode="decimal"
                            onChange={(e) => handleChange(index, "studentCapacity", e.value)}
                        />
                        <MultiSelect
                            value={room.permittedDepartments}
                            onChange={(e) => handleChange(index, "permittedDepartments", e.value)}
                            options={departments}
                            display="chip"
                            placeholder="Select Departments (optional)"
                            className="w-full md:w-20rem" />
                    </div>
                ))}
                <div className="button-section">
                    <button onClick={() => setCurrentStep(1)}>Go Back</button>
                    <button onClick={addRoom}>Add Another</button>
                    <div className="tooltip-container">
                        <button disabled={!enableButton} onClick={() => setCurrentStep(currentStep + 1)}>Submit</button>
                        {!enableButton && (
                            <span className="tooltip">
                                {rooms.some(r => r.studentCapacity == "" || !r.studentCapacity || r.studentCapacity <= 0)
                                    ? "All rooms must have a non-negative capacity."
                                    : hasDuplicates ? "No duplicate rooms" : "Fill in a name and capacity for all rooms."
                                }
                            </span>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
}