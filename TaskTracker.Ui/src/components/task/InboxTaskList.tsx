import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../../stores/RootStore";
import TaskList from "./TaskList";
import TaskCreationPanel from "./TaskCreationPanel";

const InboxTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getInboxIncompletedTasks();
    const completedTasks = taskStore.getInboxCompletedTasks();

    return (
        <>
            <div style={{ paddingLeft: 10, paddingTop: 10 }}>
                <strong>Inbox</strong>
            </div>
            <TaskCreationPanel />
            <Divider />
            <TaskList
                incompletedTasks={incompletedTasks}
                completedTasks={completedTasks}
            />
        </>
    );
});

export default InboxTaskList;
