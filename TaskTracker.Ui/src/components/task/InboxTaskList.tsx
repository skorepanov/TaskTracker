import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationPanel from "./TaskCreationPanel";

const InboxTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getInboxIncompletedTasks();
    const completedTasks = taskStore.getInboxCompletedTasks();

    return (
        <>
            <div className="task-list-header">
                <strong>Inbox</strong>
                <TaskCreationPanel />
            </div>
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
            />
        </>
    );
});

export default InboxTaskList;
