import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationModalButton from "./TaskCreationModalButton";

const InboxTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getInboxIncompletedTasks();
    const completedTasks = taskStore.getInboxCompletedTasks();

    return (
        <>
            <div style={{ float: "left" }}>
                <strong>Inbox</strong>
            </div>
            <div style={{ float: "right" }}>
                <TaskCreationModalButton />
            </div>
            <Divider />
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
            />
        </>
    );
});

export default InboxTaskList;
