import React from "react";
import { observer } from "mobx-react-lite";
import { Divider } from "antd";
import { useStore } from "../stores/RootStore";
import TaskListView from "./TaskListView";

const InboxTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTaskComponents = taskStore
        .getInboxIncompletedTasks()
        .map(t => (
            <TaskListView
                key={`inbox-${t.id}`}
                task={t}
            />
        ));

    const completedTaskComponents = taskStore
        .getInboxCompletedTasks()
        .map(t => (
            <TaskListView
                key={`inbox-${t.id}`}
                task={t}
            />
        ));

    return (
        <>
            <strong>Inbox</strong>
            <Divider />
            {incompletedTaskComponents}
            <Divider />
            {completedTaskComponents}
        </>
    );
});

export default InboxTaskList;
