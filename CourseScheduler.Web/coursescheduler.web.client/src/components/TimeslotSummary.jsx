import { useState, useContext } from "react";

function convertMilitaryToStandard(time) {
    let [hours, minutes] = time.split(":").map(Number);
    let period = hours >= 12 ? "PM" : "AM";

    hours = hours % 12 || 12;

    return `${hours}:${String(minutes).padStart(2, "0")} ${period}`;
}

export default function TimeslotSummary(props) {
    const [displaying, setDisplaying] = useState(false);

    const expressions = props.expressions;
    console.log(expressions);
    const dayShortener = {
        "Monday": "M",
        "Tuesday": "Tu",
        "Wednesday": "W",
        "Thursday": "Th",
        "Friday": "F",
    };
    let expressionsDescrs = expressions.map(expression => {
        const slotSelections = Object.values(expression);

        let canShortenDescription = true;
        if (slotSelections.every(s => s.length === 1)) {
            const firstStart = slotSelections[0][0].start;
            const firstEnd = slotSelections[0][0].end;
            for (let i = 1; i < slotSelections.length; i++) {
                if (slotSelections[i][0].start !== firstStart || slotSelections[i][0].end !== firstEnd) {
                    canShortenDescription = false;
                }
            }
        } else {
            canShortenDescription = false;
        }

        if (canShortenDescription) {
            return [Object.keys(expression).map(d => dayShortener[d]).join("") + ": " + convertMilitaryToStandard(slotSelections[0][0].start) + "-" + convertMilitaryToStandard(slotSelections[0][0].end)];
        }

        return Object.keys(expression).map(day => {
            const allTimes = expression[day];
            const commaSepTimes = allTimes.map(t => convertMilitaryToStandard(t.start) + "-" + convertMilitaryToStandard(t.end)).join();
            return dayShortener[day] + ": " + commaSepTimes;
        });
    });

    return (
        <div className="expression-description">
            <ul>
                {
                    expressionsDescrs.map(descr => {
                        if (descr.length === 1) {
                            return <li>{descr[0]}</li>
                        } else {
                            return (
                                <ul>
                                    {descr.map(d => <li>{d}</li>)}
                                </ul>
                            );
                        }
                    })
            }
            </ul>
        </div>
    )
}