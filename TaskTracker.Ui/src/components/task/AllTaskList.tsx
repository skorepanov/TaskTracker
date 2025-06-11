import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationPanel from "./TaskCreationPanel";

const AllTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getIncompletedTasks(true);
    const completedTasks = taskStore.getCompletedTasks(true);

    return (
        <>
            <div className="task-list-header">
                <strong>Все задачи</strong>
                <TaskCreationPanel />
            </div>
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
                shouldShowFolder={true}
            />
        </>
    );
});

export default AllTaskList;
