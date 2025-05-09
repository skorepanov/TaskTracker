import React, { useState } from 'react';
import { Checkbox } from 'antd';
import type { CheckboxChangeEvent } from 'antd/es/checkbox';
import ITask from '../interfaces/ITask';

interface ITaskProps {
    task: ITask;
    completeTask: (task: ITask) => Promise<void>;
    incompleteTask: (task: ITask) => Promise<void>;
}

const Task: React.FC<ITaskProps> = (props) => {
    const isTaskCompleted = props.task.completedDateTime !== null;

    const [isCompleted, setIsCompleted] = useState<boolean>(isTaskCompleted);

    const textColor = isTaskCompleted
        ? 'green'
        : 'black';

    const completedDateTimeAsText = isTaskCompleted
        ? <>Выполнено: {props.task.completedDateTime}<br /></>
        : null;

    const dueDateTimeAsText = props.task.dueDateTime !== null
        ? <>Срок выполнения: {props.task.dueDateTime}</>
        : null;

    const handleCompletedChange = async (event: CheckboxChangeEvent) => {
        const isCompletedNew = event.target.checked;

        setIsCompleted(isCompletedNew);

        if (isCompletedNew) {
            await props.completeTask(props.task);
        } else {
            await props.incompleteTask(props.task);
        }
    }

    return (
        <div style={{ color: textColor, marginBottom: 10 }}>
            <Checkbox
                checked={isCompleted}
                onChange={handleCompletedChange}
                style={{ marginRight: 5 }}
            />
            [{props.task.id}] {props.task.title}<br />
            <i>{props.task.description}</i><br />
            {completedDateTimeAsText}
            {dueDateTimeAsText}
        </div>
    );
}

export default Task;
