import React from 'react';
import { Checkbox } from 'antd';
import type { CheckboxChangeEvent } from 'antd/es/checkbox';
import ITask from '../interfaces/ITask';

class Task extends React.Component<ITaskProps, ITaskState> {
    constructor(props: ITaskProps) {
        super(props);

        this.state = {
            isCompleted: props.task.completedDateTime !== null,
        }
    }

    onCompletedChange = async (e: CheckboxChangeEvent) => {
        const isCompleted = e.target.checked;

        this.setState({ isCompleted: isCompleted });

        if (isCompleted) {
            await this.props.completeTask(this.props.task);
        } else {
            await this.props.incompleteTask(this.props.task);
        }
    }

    render() {
        const task = this.props.task;

        const textColor = this.state.isCompleted ? 'green' : 'black';

        const description = task.description?.length > 0
            ? <><i>{task.description}</i><br /></>
            : null;

        const completedDateTime = this.state.isCompleted
            ? <>Выполнено: {task.completedDateTime}<br /></>
            : null;

        const dueDateTime = task.dueDateTime !== null
            ? <span>Срок выполнения: {task.dueDateTime}</span>
            : null;

        return (
            <div style={{ color: textColor, marginBottom: 10 }}>
                <Checkbox
                    checked={this.state.isCompleted}
                    onChange={this.onCompletedChange}
                    style={{ marginRight: 5 }}
                />
                [{task.id}] {task.title}<br />
                {description}
                {completedDateTime}
                {dueDateTime}
            </div>
        );
    }
}

export default Task;

interface ITaskProps {
    task: ITask;
    completeTask: (task: ITask) => Promise<void>;
    incompleteTask: (task: ITask) => Promise<void>;
}

interface ITaskState {
    isCompleted: boolean;
}