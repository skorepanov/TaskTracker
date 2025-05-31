import React from "react";
import { Link, useLocation } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Dropdown, Menu, MenuProps, Tag } from "antd";
import { useStore } from "../stores/RootStore";
import IFolder from "../interfaces/IFolder";
import FolderCreationModalButton from "./FolderCreationModalButton";
import TagCreationModalButton from "./TagCreationModalButton";

type MenuItem = Required<MenuProps>["items"][number];

const MainMenu: React.FC = observer(() => {
    const location = useLocation();

    const { taskStore, folderStore, tagStore } = useStore();

    const allIncompletedTaskCount = taskStore.incompletedTasks.length;

    const todayIncompletedTaskCount =
        taskStore.getTodayIncompletedTasks().length;

    const inboxIncompletedTasksCount =
        taskStore.getInboxIncompletedTasks().length;

    const getMenuItem = (
        link: string,
        title: string,
        incompletedTaskCount?: number
    ) => {
        return (
            <Link to={link}>
                <div style={{ float: "left" }}>{title}</div>
                <div style={{ float: "right", color: "grey" }}>
                    {incompletedTaskCount}
                </div>
            </Link>
        );
    };

    const getFolderRootMenuItem = () => {
        return (
            <>
                <div style={{ float: "left" }}>Папки</div>
                <div
                    onClick={e => e.stopPropagation()}
                    style={{ float: "right" }}>
                    <FolderCreationModalButton />
                </div>
            </>
        );
    };

    const getFolderMenuItem = (folder: IFolder) => {
        const folderIncompleteTaskCount = taskStore.incompletedTasks.filter(
            t => t.folderId === folder.id
        ).length;

        const handleDeleteFolderButtonClick = async () => {
            await folderStore.deleteFolder(folder);
        };

        const contextMenu = {
            items: [
                {
                    label: "Удалить папку",
                    key: "deleteFolder",
                },
            ],
            onClick: handleDeleteFolderButtonClick,
        };

        return (
            <Dropdown
                key={folder.id}
                menu={contextMenu}
                trigger={["contextMenu"]}>
                {getMenuItem(
                    `/folders/${folder.id}`,
                    folder.title,
                    folderIncompleteTaskCount
                )}
            </Dropdown>
        );
    };

    const getFolderMenuItems = () => {
        return folderStore.folders.map(f => {
            return {
                key: `/folders/${f.id}`,
                label: getFolderMenuItem(f),
            };
        });
    };

    const getTagRootMenuItem = () => {
        return (
            <>
                <div style={{ float: "left" }}>Теги</div>
                <div
                    onClick={e => e.stopPropagation()}
                    style={{ float: "right" }}>
                    <TagCreationModalButton />
                </div>
            </>
        );
    };

    const getTagMenuItems = () => {
        return tagStore.tags.map(t => {
            return {
                key: `/tags/${t.id}`,
                label: (
                    <Link to={`/tags/${t.id}`}>
                        <Tag color={`#${t.color}`}>
                            <div style={{ mixBlendMode: "difference" }}>
                                {t.title}
                            </div>
                        </Tag>
                    </Link>
                ),
            };
        });
    };

    const mainMenuItems: MenuItem[] = [
        {
            key: "/all",
            label: getMenuItem("/all", "Все задачи", allIncompletedTaskCount),
        },
        {
            key: "/today",
            label: getMenuItem("/today", "Сегодня", todayIncompletedTaskCount),
        },
        {
            key: "/inbox",
            label: getMenuItem("/inbox", "Inbox", inboxIncompletedTasksCount),
        },
        {
            type: "divider",
        },
        {
            key: "/folders",
            label: getFolderRootMenuItem(),
            children: getFolderMenuItems(),
        },
        {
            type: "divider",
        },
        {
            key: "/tags",
            label: getTagRootMenuItem(),
            children: getTagMenuItems(),
        },
        {
            type: "divider",
        },
        {
            key: "/trash",
            label: getMenuItem("/trash", "Корзина"),
        },
    ];

    return (
        <Menu
            items={mainMenuItems}
            mode="inline"
            selectedKeys={[location.pathname]}
            defaultOpenKeys={["/folders"]}
            style={{ width: "270px" }}
        />
    );
});

export default MainMenu;
